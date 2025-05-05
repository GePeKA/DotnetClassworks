// dart format width=80
// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'pagination_bloc.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;
/// @nodoc
mixin _$PaginationState {





@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is PaginationState);
}


@override
int get hashCode => runtimeType.hashCode;

@override
String toString() {
  return 'PaginationState()';
}


}

/// @nodoc
class $PaginationStateCopyWith<$Res>  {
$PaginationStateCopyWith(PaginationState _, $Res Function(PaginationState) __);
}


/// @nodoc


class Loading implements PaginationState {
  const Loading();
  






@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is Loading);
}


@override
int get hashCode => runtimeType.hashCode;

@override
String toString() {
  return 'PaginationState.loading()';
}


}




/// @nodoc


class Normal implements PaginationState {
  const Normal({final  List<PostEntity> posts = const [], this.isLoading = false, this.hasMore = true}): _posts = posts;
  

 final  List<PostEntity> _posts;
@JsonKey() List<PostEntity> get posts {
  if (_posts is EqualUnmodifiableListView) return _posts;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_posts);
}

@JsonKey() final  bool isLoading;
@JsonKey() final  bool hasMore;

/// Create a copy of PaginationState
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$NormalCopyWith<Normal> get copyWith => _$NormalCopyWithImpl<Normal>(this, _$identity);



@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is Normal&&const DeepCollectionEquality().equals(other._posts, _posts)&&(identical(other.isLoading, isLoading) || other.isLoading == isLoading)&&(identical(other.hasMore, hasMore) || other.hasMore == hasMore));
}


@override
int get hashCode => Object.hash(runtimeType,const DeepCollectionEquality().hash(_posts),isLoading,hasMore);

@override
String toString() {
  return 'PaginationState.normal(posts: $posts, isLoading: $isLoading, hasMore: $hasMore)';
}


}

/// @nodoc
abstract mixin class $NormalCopyWith<$Res> implements $PaginationStateCopyWith<$Res> {
  factory $NormalCopyWith(Normal value, $Res Function(Normal) _then) = _$NormalCopyWithImpl;
@useResult
$Res call({
 List<PostEntity> posts, bool isLoading, bool hasMore
});




}
/// @nodoc
class _$NormalCopyWithImpl<$Res>
    implements $NormalCopyWith<$Res> {
  _$NormalCopyWithImpl(this._self, this._then);

  final Normal _self;
  final $Res Function(Normal) _then;

/// Create a copy of PaginationState
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') $Res call({Object? posts = null,Object? isLoading = null,Object? hasMore = null,}) {
  return _then(Normal(
posts: null == posts ? _self._posts : posts // ignore: cast_nullable_to_non_nullable
as List<PostEntity>,isLoading: null == isLoading ? _self.isLoading : isLoading // ignore: cast_nullable_to_non_nullable
as bool,hasMore: null == hasMore ? _self.hasMore : hasMore // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}


}

/// @nodoc


class Error implements PaginationState {
  const Error({required this.message, required this.statusCode, final  List<PostEntity> posts = const []}): _posts = posts;
  

 final  String message;
 final  int? statusCode;
 final  List<PostEntity> _posts;
@JsonKey() List<PostEntity> get posts {
  if (_posts is EqualUnmodifiableListView) return _posts;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_posts);
}


/// Create a copy of PaginationState
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$ErrorCopyWith<Error> get copyWith => _$ErrorCopyWithImpl<Error>(this, _$identity);



@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is Error&&(identical(other.message, message) || other.message == message)&&(identical(other.statusCode, statusCode) || other.statusCode == statusCode)&&const DeepCollectionEquality().equals(other._posts, _posts));
}


@override
int get hashCode => Object.hash(runtimeType,message,statusCode,const DeepCollectionEquality().hash(_posts));

@override
String toString() {
  return 'PaginationState.error(message: $message, statusCode: $statusCode, posts: $posts)';
}


}

/// @nodoc
abstract mixin class $ErrorCopyWith<$Res> implements $PaginationStateCopyWith<$Res> {
  factory $ErrorCopyWith(Error value, $Res Function(Error) _then) = _$ErrorCopyWithImpl;
@useResult
$Res call({
 String message, int? statusCode, List<PostEntity> posts
});




}
/// @nodoc
class _$ErrorCopyWithImpl<$Res>
    implements $ErrorCopyWith<$Res> {
  _$ErrorCopyWithImpl(this._self, this._then);

  final Error _self;
  final $Res Function(Error) _then;

/// Create a copy of PaginationState
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') $Res call({Object? message = null,Object? statusCode = freezed,Object? posts = null,}) {
  return _then(Error(
message: null == message ? _self.message : message // ignore: cast_nullable_to_non_nullable
as String,statusCode: freezed == statusCode ? _self.statusCode : statusCode // ignore: cast_nullable_to_non_nullable
as int?,posts: null == posts ? _self._posts : posts // ignore: cast_nullable_to_non_nullable
as List<PostEntity>,
  ));
}


}

// dart format on
