import 'package:mvvm_clean_flutter/domain/entities/post.dart';

abstract class PostRepository {
  Future<PostEntity> getPost(int id);
  Future<List<PostEntity>> getPosts(int page, int limit);
}