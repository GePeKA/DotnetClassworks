import 'package:dio/dio.dart';

class DioClient {
  final Dio dio;

  DioClient() : dio = Dio() {
    dio.options.baseUrl = "https://jsonplaceholder.typicode.com";
    dio.options.connectTimeout = const Duration(seconds: 30);
    dio.options.receiveTimeout = const Duration(seconds: 30);
    dio.options.responseType = ResponseType.json;
  }

  Dio get instance => dio;
}