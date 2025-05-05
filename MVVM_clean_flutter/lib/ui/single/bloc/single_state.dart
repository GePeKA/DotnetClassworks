part of 'single_bloc.dart';

@freezed
sealed class SingleState with _$SingleState {
  const factory SingleState.normal({
    PostEntity? post,
    @Default(false) bool isLoading,
  }) = Normal;

  const factory SingleState.loading() = Loading;

  const factory SingleState.error({
    required String message,
    required int? statusCode,
  }) = Error;
}