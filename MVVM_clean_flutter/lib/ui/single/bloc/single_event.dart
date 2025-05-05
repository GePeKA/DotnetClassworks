part of 'single_bloc.dart';

abstract class SingleEvent {
  const SingleEvent();
}

class LoadSinglePost extends SingleEvent {
  final int id;

  const LoadSinglePost(this.id);
}