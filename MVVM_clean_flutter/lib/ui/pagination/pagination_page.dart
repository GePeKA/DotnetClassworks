import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:mvvm_clean_flutter/core/di.dart';
import 'package:mvvm_clean_flutter/core/widgets/loading_indicator.dart';
import 'package:mvvm_clean_flutter/domain/entities/post.dart';
import 'package:mvvm_clean_flutter/ui/pagination/bloc/pagination_bloc.dart';

class PaginationPage extends StatelessWidget {
  const PaginationPage({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (context) => getIt<PaginationBloc>()..add(LoadPaginationPosts()),
      child: Scaffold(
        appBar: AppBar(title: const Text('Posts Pagination')),
        body: const _PaginationPageBody(),
      ),
    );
  }
}

class _PaginationPageBody extends StatelessWidget {
  const _PaginationPageBody();

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<PaginationBloc, PaginationState>(
      builder: (context, state) {
        return switch (state) {
          Loading() => const LoadingIndicator(),
          Normal(:final posts, :final isLoading, :final hasMore) =>
              _buildContent(posts, isLoading, hasMore, context),
          Error(:final message, :final statusCode, :final posts) =>
              _buildErrorWithPosts(message, statusCode, posts, context),
        };
      },
    );
  }

  Widget _buildContent(
      List<PostEntity> posts,
      bool isLoading,
      bool hasMore,
      BuildContext context,
      ) {
    return NotificationListener<ScrollNotification>(
      onNotification: (scrollInfo) {
        if (!isLoading &&
            hasMore &&
            !scrollInfo.metrics.outOfRange &&
            scrollInfo.metrics.pixels == scrollInfo.metrics.maxScrollExtent) {
          context.read<PaginationBloc>().add(LoadPaginationPosts());
        }
        return false;
      },
      child: ListView.builder(
        key: const PageStorageKey('pagination_list'),
        itemCount: hasMore ? posts.length + 1 : posts.length,
        itemBuilder: (context, index) {
          if (index >= posts.length) {
            return const Padding(
              padding: EdgeInsets.all(16),
              child: LoadingIndicator(),
            );
          }
          return _buildPostItem(posts[index]);
        },
      ),
    );
  }

  Widget _buildErrorWithPosts(
      String message,
      int? statusCode,
      List<PostEntity> posts,
      BuildContext context,
      ) {
    return Column(
      children: [
        if (posts.isNotEmpty)
          Expanded(
            child: _buildContent(posts, false, false, context),
          ),
        Padding(
          padding: const EdgeInsets.all(16),
          child: Center(
            child: Column(
              children: [
                Text(
                  'Ошибка ${statusCode ?? ''}: $message',
                  style: TextStyle(color: Colors.red[700]),
                  textAlign: TextAlign.center,
                ),
                const SizedBox(height: 16),
                ElevatedButton(
                  onPressed: () => context.read<PaginationBloc>().add(LoadPaginationPosts()),
                  child: const Text('Повторить'),
                ),
              ],
            ),
          )
        ),
      ],
    );
  }

  Widget _buildPostItem(PostEntity post) {
    return Card(
      margin: const EdgeInsets.all(8),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(post.title, style: const TextStyle(
              fontSize: 18,
              fontWeight: FontWeight.bold,
            )),
            const SizedBox(height: 8),
            Text(post.body),
            const SizedBox(height: 8),
            Text('Post ID: ${post.id}', style: const TextStyle(
              fontSize: 12,
              color: Colors.grey,
            )),
          ],
        ),
      ),
    );
  }
}