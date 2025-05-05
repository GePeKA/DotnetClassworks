import 'package:mvvm_clean_flutter/domain/entities/post.dart';
import 'package:mvvm_clean_flutter/domain/repositories/post_repository.dart';

class GetPosts {
  final PostRepository repository;

  GetPosts({required this.repository});

  Future<List<PostEntity>> call(int page, int limit) async {
    return await repository.getPosts(page, limit);
  }
}