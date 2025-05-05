class ServerException implements Exception {
  final String message;
  final int statusCode;

  ServerException({required this.message, this.statusCode = 500});

  @override
  String toString() => 'ServerException: $message (code: $statusCode)';
}

class CacheException implements Exception {
  final String message;

  CacheException({required this.message});

  @override
  String toString() => 'CacheException: $message';
}