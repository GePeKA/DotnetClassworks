import 'package:mvvm_clean_flutter/domain/entities/post.dart';
import 'package:mvvm_clean_flutter/domain/repositories/post_repository.dart';

class GetPost {
  final PostRepository repository;

  GetPost({required this.repository});

  Future<PostEntity> call(int id) async {
    return await repository.getPost(id);
  }
}