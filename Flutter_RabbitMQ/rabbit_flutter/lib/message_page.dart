import 'dart:async';
import 'package:flutter/material.dart';
import 'package:rabbit_flutter/login_page.dart';
import 'package:rabbit_flutter/rabbitmq_service.dart';

class MessagePage extends StatefulWidget {
  final String username;

  const MessagePage({super.key, required this.username});

  @override
  State<MessagePage> createState() => _MessagePageState();
}

class _MessagePageState extends State<MessagePage> {
  final List<String> _messages = [];
  late RabbitMQService _rabbitMQService;
  late StreamSubscription _subscription;

  @override
  void initState() {
    super.initState();

    _rabbitMQService = RabbitMQService(username: widget.username);
    _initRabbit();
  }

  Future<void> _initRabbit() async {
    final consumer = await _rabbitMQService.connectAndSubscribe();
    _subscription = consumer.listen((message) {
      setState(() {
        _messages.insert(0, message.payloadAsJson["message"]["text"]);
      });
    });
  }

  void _logout() {
    _rabbitMQService.sendLogout();
    _rabbitMQService.close();
    _subscription.cancel();
    Navigator.pushReplacement(
      context,
      MaterialPageRoute(builder: (_) => const LoginPage()),
    );
  }

  @override
  void dispose() {
    _subscription.cancel();
    _rabbitMQService.close();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Сообщения'),
        actions: [
          IconButton(
            icon: const Icon(Icons.logout),
            onPressed: _logout,
          )
        ],
      ),
      body: ListView.separated(
        itemCount: _messages.length,
        itemBuilder: (_, index) => ListTile(title: Text(_messages[index])),
        separatorBuilder: (BuildContext context, int index) => Divider(),
      ),
    );
  }
}
