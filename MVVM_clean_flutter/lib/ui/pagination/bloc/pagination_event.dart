part of 'pagination_bloc.dart';

abstract class PaginationEvent{
  const PaginationEvent();
}

class LoadPaginationPosts extends PaginationEvent {}