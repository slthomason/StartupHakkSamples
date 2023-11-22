[MemoryDiagnoser]
public class ConcatenateExamples
{
  [Benchmark]
  public string StringConcatExample()
  {
    return string.Concat(9, '-', 6, '-', 2022);
  }

  [Benchmark]
  public string StringConcatExampleToString()
  {
    return string.Concat(9.ToString(), '-', 6.ToString(), '-', 2022.ToString());
  }

  [Benchmark]
  public string StringBuilderConcatExample()
  {
    var sb = new StringBuilder();
    sb.Append(2022);
    sb.Append('-');
    sb.Append(6);
    sb.Append('-');
    sb.Append(9);
    return sb.ToString();
  }

  
  [Benchmark]
  public string StringFormatExample()
  {
    return string.Format('{0}-{1}-{2}', 2022, 6, 9);
  }

  [Benchmark]
  public string InterpolationExample()
  {
    return $"{2022}-{6}-{9}";
  }

}