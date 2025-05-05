import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:mvvm_clean_flutter/core/errors.dart';
import 'package:mvvm_clean_flutter/domain/entities/post.dart';
import 'package:mvvm_clean_flutter/domain/use_cases/get_post.dart';
import 'package:freezed_annotation/freezed_annotation.dart';

part 'single_event.dart';
part 'single_state.dart';
part 'single_bloc.freezed.dart';

class SingleBloc extends Bloc<SingleEvent, SingleState> {
  final GetPost getPost;

  SingleBloc(this.getPost) : super(const SingleState.loading()) {
    on<LoadSinglePost>(_onLoadSinglePost);
  }

  Future<void> _onLoadSinglePost(
      LoadSinglePost event,
      Emitter<SingleState> emit,
      ) async {
    emit(const SingleState.normal(post:null, isLoading: true));
    await Future.delayed(Duration(seconds: 2));
    try {
      final post = await getPost(event.id);
      emit(SingleState.normal(post: post));
    } on ServerException catch (e) {
      emit(SingleState.error(
        message: e.message,
        statusCode: e.statusCode
      ));
    } catch (e) {
      emit(SingleState.error(
        message: "Failed to load posts",
        statusCode: 500
      ));
    }
  }
}
