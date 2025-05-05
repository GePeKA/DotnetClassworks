import 'package:flutter/material.dart';
import 'package:mvvm_clean_flutter/app.dart';
import 'package:mvvm_clean_flutter/core/di.dart' as di;

void main() {
  WidgetsFlutterBinding.ensureInitialized();
  di.init();
  runApp(const MyApp());
}
