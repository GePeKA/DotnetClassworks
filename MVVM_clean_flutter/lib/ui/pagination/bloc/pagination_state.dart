part of 'pagination_bloc.dart';

@freezed
sealed class PaginationState with _$PaginationState {
  const factory PaginationState.loading() = Loading;

  const factory PaginationState.normal({
    @Default([]) List<PostEntity> posts,
    @Default(false) bool isLoading,
    @Default(true) bool hasMore,
  }) = Normal;

  const factory PaginationState.error({
    required String message,
    required int? statusCode,
    @Default([]) List<PostEntity> posts,
  }) = Error;
}