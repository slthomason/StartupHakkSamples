using Prallelism;

await AsyncParallelism.Main().ConfigureAwait(false);
DataParallelism.Main();
TaskParallelism.Main();
PipelineParallelism.Main();
