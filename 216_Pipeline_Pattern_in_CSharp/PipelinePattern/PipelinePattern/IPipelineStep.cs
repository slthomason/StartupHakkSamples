namespace PipelinePattern
{
    public interface IPipelineStep<T>
    {
        T Process(T input);
    }
}