var parallelQuery = from item in data.AsParallel()
                    where item.SomeCondition
                    select item;

data.AsParallel().ForAll(item => ProcessItem(item));
