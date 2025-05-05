import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter/material.dart';
import 'package:mvvm_clean_flutter/core/di.dart';
import 'package:mvvm_clean_flutter/core/widgets/loading_indicator.dart';
import 'package:mvvm_clean_flutter/domain/entities/post.dart';
import 'package:mvvm_clean_flutter/ui/single/bloc/single_bloc.dart';

class SinglePage extends StatelessWidget {
  const SinglePage({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (context) => getIt<SingleBloc>()..add(const LoadSinglePost(1)),
      child: Scaffold(
        appBar: AppBar(title: const Text('Single Post')),
        body: const _SinglePageBody(),
      ),
    );
  }
}

class _SinglePageBody extends StatelessWidget {
  const _SinglePageBody();

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<SingleBloc, SingleState>(
      builder: (context, state) {
        return switch (state) {
          Normal(post: final post, isLoading: true) => const LoadingIndicator(),
          Normal(post: final post?) => _buildPostContent(post),
          Loading() => const LoadingIndicator(),
          Error(message: final message, statusCode: final statusCode) =>
              _buildErrorWidget(message, statusCode, context),
          _ => Text("asd")
        };
      },
    );
  }

  Widget _buildPostContent(PostEntity post) {
    return Padding(
      padding: const EdgeInsets.all(16.0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text('ID: ${post.id}', style: const TextStyle(fontSize: 18)),
          Text('User ID: ${post.userId}', style: const TextStyle(fontSize: 18)),
          const SizedBox(height: 16),
          Text('Title: ${post.title}',
              style: const TextStyle(fontSize: 20, fontWeight: FontWeight.bold)),
          const SizedBox(height: 16),
          Text('Body: ${post.body}', style: const TextStyle(fontSize: 16)),
        ],
      ),
    );
  }

  Widget _buildErrorWidget(String message, int? statusCode, BuildContext context) {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Text(
            statusCode != null ? 'Error $statusCode: $message' : message,
            style: const TextStyle(color: Colors.red),
          ),
          const SizedBox(height: 16),
          ElevatedButton(
            onPressed: () => context.read<SingleBloc>().add(
              const LoadSinglePost(1),
            ),
            child: const Text('Retry'),
          ),
        ],
      ),
    );
  }
}