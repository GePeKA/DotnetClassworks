import 'package:mvvm_clean_flutter/data/services/post_service.dart';
import 'package:mvvm_clean_flutter/domain/entities/post.dart';
import 'package:mvvm_clean_flutter/domain/repositories/post_repository.dart';

class PostRepositoryImpl implements PostRepository {
  final PostService postService;

  PostRepositoryImpl({required this.postService});

  @override
  Future<PostEntity> getPost(int id) async {
    final post = await postService.getPost(id);
    return PostEntity(
      id: post.id,
      userId: post.userId,
      title: post.title,
      body: post.body,
    );
  }

  @override
  Future<List<PostEntity>> getPosts(int page, int limit) async {
    final posts = await postService.getPosts(page, limit);
    return posts
        .map((post) => PostEntity(
          id: post.id,
          userId: post.userId,
          title: post.title,
          body: post.body,
        ))
        .toList();
  }
}