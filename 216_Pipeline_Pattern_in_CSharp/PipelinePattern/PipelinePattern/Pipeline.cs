namespace PipelinePattern
{
    public class Pipeline<T>
    {
        private readonly List<IPipelineStep<T>> _steps = new List<IPipelineStep<T>>();

        public Pipeline<T> AddStep(IPipelineStep<T> step)
        {
            _steps.Add(step);
            return this;
        }

        public T Execute(T input)
        {
            T result = input;
            foreach (var step in _steps)
            {
                result = step.Process(result);
            }
            return result;
        }
    }
}