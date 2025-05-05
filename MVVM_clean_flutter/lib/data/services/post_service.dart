import 'package:dio/dio.dart';
import 'package:mvvm_clean_flutter/core/errors.dart';
import 'package:mvvm_clean_flutter/data/models/post.dart';

class PostService {
  final Dio dio;

  PostService({required this.dio});

  Future<PostModel> getPost(int id) async {
    try {
      final response = await dio.get('/posts/$id');
      return PostModel.fromJson(response.data);
    } on DioException catch (e) {
      throw ServerException(
        message: e.response?.statusMessage ?? 'Failed to fetch post',
        statusCode: e.response?.statusCode ?? 500,
      );
    } catch (e) {
      throw ServerException(message: 'Unknown error occurred');
    }
  }

  Future<List<PostModel>> getPosts(int page, int limit) async {
    try {
      final response = await dio.get(
        '/posts',
        queryParameters: {'_page': page, '_limit': limit},
      );
      return (response.data as List).map((e) => PostModel.fromJson(e)).toList();
    } on DioException catch (e) {
      throw ServerException(
        message: e.response?.statusMessage ?? 'Failed to fetch posts',
        statusCode: e.response?.statusCode ?? 500,
      );
    } catch (e) {
      throw ServerException(message: 'Unknown error occurred');
    }
  }
}