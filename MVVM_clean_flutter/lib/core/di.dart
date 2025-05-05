import 'package:get_it/get_it.dart';
import 'package:mvvm_clean_flutter/core/dio_client.dart';
import 'package:mvvm_clean_flutter/data/repositories/post_repository.dart';
import 'package:mvvm_clean_flutter/data/services/post_service.dart';
import 'package:mvvm_clean_flutter/domain/repositories/post_repository.dart';
import 'package:mvvm_clean_flutter/domain/use_cases/get_post.dart';
import 'package:mvvm_clean_flutter/domain/use_cases/get_posts.dart';
import 'package:mvvm_clean_flutter/ui/pagination/bloc/pagination_bloc.dart';
import 'package:mvvm_clean_flutter/ui/single/bloc/single_bloc.dart';

final getIt = GetIt.instance;

void init() {
  // External
  getIt.registerLazySingleton<DioClient>(
    () => DioClient()
  );

  // Services
  getIt.registerLazySingleton<PostService>(
    () => PostService(dio: getIt<DioClient>().instance),
  );

  // Repositories
  getIt.registerLazySingleton<PostRepository>(
    () => PostRepositoryImpl(postService: getIt<PostService>()),
  );

  // UseCases
  getIt.registerLazySingleton<GetPost>(
    () => GetPost(repository: getIt<PostRepository>())
  );
  getIt.registerLazySingleton<GetPosts>(
    () => GetPosts(repository: getIt<PostRepository>())
  );

  // Blocs
  getIt.registerFactory<SingleBloc>(
    () => SingleBloc(getIt<GetPost>())
  );
  getIt.registerFactory<PaginationBloc>(
    () => PaginationBloc(getIt<GetPosts>()),
  );
}