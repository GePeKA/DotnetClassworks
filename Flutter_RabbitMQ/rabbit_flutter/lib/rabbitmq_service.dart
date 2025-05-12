import 'dart:convert';
import 'package:dart_amqp/dart_amqp.dart';

class RabbitMQService {
  final String username;
  late Client _client;

  RabbitMQService({required this.username});

  Future<Consumer> connectAndSubscribe() async {
    _client = Client(
      settings: ConnectionSettings(
        host: 'tcp.cloudpub.ru',
        port: 57885,
        authProvider: const PlainAuthenticator('guest', 'guest'),
      ),
    );

    final channel = await _client.channel();
    final queue = await channel.queue(
      'flutter_${username}_queue',
      durable: false,
      autoDelete: true
    );
    final simpleMessageExchange = await channel.exchange(
        "Auth_BackgroundService.Messages:SimpleMessage",
        ExchangeType.FANOUT,
        durable: true);
    final bindedQueue = await queue.bind(simpleMessageExchange, "");

    return bindedQueue.consume(
      consumerTag: 'flutter_consumer_$username',
    );
  }

  void sendLogout() async {
    final channel = await _client.channel();
    final exchange = await channel.exchange('logout_exchange', ExchangeType.FANOUT);

    final message = jsonEncode({'username': username});
    exchange.publish(message, null);
  }

  void close() {
    _client.close();
  }
}
