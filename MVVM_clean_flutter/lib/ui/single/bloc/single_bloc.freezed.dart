// dart format width=80
// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'single_bloc.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;
/// @nodoc
mixin _$SingleState {





@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is SingleState);
}


@override
int get hashCode => runtimeType.hashCode;

@override
String toString() {
  return 'SingleState()';
}


}

/// @nodoc
class $SingleStateCopyWith<$Res>  {
$SingleStateCopyWith(SingleState _, $Res Function(SingleState) __);
}


/// @nodoc


class Normal implements SingleState {
  const Normal({this.post, this.isLoading = false});
  

 final  PostEntity? post;
@JsonKey() final  bool isLoading;

/// Create a copy of SingleState
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$NormalCopyWith<Normal> get copyWith => _$NormalCopyWithImpl<Normal>(this, _$identity);



@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is Normal&&(identical(other.post, post) || other.post == post)&&(identical(other.isLoading, isLoading) || other.isLoading == isLoading));
}


@override
int get hashCode => Object.hash(runtimeType,post,isLoading);

@override
String toString() {
  return 'SingleState.normal(post: $post, isLoading: $isLoading)';
}


}

/// @nodoc
abstract mixin class $NormalCopyWith<$Res> implements $SingleStateCopyWith<$Res> {
  factory $NormalCopyWith(Normal value, $Res Function(Normal) _then) = _$NormalCopyWithImpl;
@useResult
$Res call({
 PostEntity? post, bool isLoading
});




}
/// @nodoc
class _$NormalCopyWithImpl<$Res>
    implements $NormalCopyWith<$Res> {
  _$NormalCopyWithImpl(this._self, this._then);

  final Normal _self;
  final $Res Function(Normal) _then;

/// Create a copy of SingleState
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') $Res call({Object? post = freezed,Object? isLoading = null,}) {
  return _then(Normal(
post: freezed == post ? _self.post : post // ignore: cast_nullable_to_non_nullable
as PostEntity?,isLoading: null == isLoading ? _self.isLoading : isLoading // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}


}

/// @nodoc


class Loading implements SingleState {
  const Loading();
  






@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is Loading);
}


@override
int get hashCode => runtimeType.hashCode;

@override
String toString() {
  return 'SingleState.loading()';
}


}




/// @nodoc


class Error implements SingleState {
  const Error({required this.message, required this.statusCode});
  

 final  String message;
 final  int? statusCode;

/// Create a copy of SingleState
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$ErrorCopyWith<Error> get copyWith => _$ErrorCopyWithImpl<Error>(this, _$identity);



@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is Error&&(identical(other.message, message) || other.message == message)&&(identical(other.statusCode, statusCode) || other.statusCode == statusCode));
}


@override
int get hashCode => Object.hash(runtimeType,message,statusCode);

@override
String toString() {
  return 'SingleState.error(message: $message, statusCode: $statusCode)';
}


}

/// @nodoc
abstract mixin class $ErrorCopyWith<$Res> implements $SingleStateCopyWith<$Res> {
  factory $ErrorCopyWith(Error value, $Res Function(Error) _then) = _$ErrorCopyWithImpl;
@useResult
$Res call({
 String message, int? statusCode
});




}
/// @nodoc
class _$ErrorCopyWithImpl<$Res>
    implements $ErrorCopyWith<$Res> {
  _$ErrorCopyWithImpl(this._self, this._then);

  final Error _self;
  final $Res Function(Error) _then;

/// Create a copy of SingleState
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') $Res call({Object? message = null,Object? statusCode = freezed,}) {
  return _then(Error(
message: null == message ? _self.message : message // ignore: cast_nullable_to_non_nullable
as String,statusCode: freezed == statusCode ? _self.statusCode : statusCode // ignore: cast_nullable_to_non_nullable
as int?,
  ));
}


}

// dart format on
