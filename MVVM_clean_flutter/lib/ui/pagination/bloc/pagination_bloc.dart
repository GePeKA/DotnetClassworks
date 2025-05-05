import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:mvvm_clean_flutter/core/errors.dart';
import 'package:mvvm_clean_flutter/domain/entities/post.dart';
import 'package:mvvm_clean_flutter/domain/use_cases/get_posts.dart';

part 'pagination_event.dart';
part 'pagination_state.dart';
part 'pagination_bloc.freezed.dart';

class PaginationBloc extends Bloc<PaginationEvent, PaginationState> {
  final GetPosts getPosts;
  int page = 1;
  static const int limit = 25;

  PaginationBloc(this.getPosts) : super(PaginationState.loading()) {
    on<LoadPaginationPosts>(_onLoadPaginationPosts);
  }

  Future<void> _onLoadPaginationPosts(
      LoadPaginationPosts event,
      Emitter<PaginationState> emit,
      ) async {
    final currentState = state;
    List<PostEntity> oldPosts = switch (currentState){
      Normal() => currentState.posts,
      Error() => currentState.posts,
      Loading() => []
    };

    emit(Normal(
      posts: oldPosts,
      isLoading: true,
      hasMore: true
    ));

    try {
      await Future.delayed(Duration(seconds: 1));
      final newPosts = await getPosts(page, limit);
      page++;

      emit(Normal(
        posts: [...oldPosts, ...newPosts],
        isLoading: false,
        hasMore: newPosts.length == limit,
      ));
    } on ServerException catch (e) {
      emit(Error(
        message: e.message,
        statusCode: e.statusCode,
        posts: oldPosts,
      ));
    } catch (e) {
      emit(Error(
        message: 'Failed to load posts',
        statusCode: 500,
        posts: oldPosts,
      ));
    }
  }
}