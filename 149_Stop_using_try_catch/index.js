//Using Try-Catch

app.post("/users", async (req: Request, res: Response) => {
    try {
      const userDto: UserDto = userValidator.validateUserDto(req.body);
      const userEntity: UserEntity = userMapper.fromDtoToEntity(userDto);
      const savedUser = await userRepository.save(userEntity);
      return res.status(201).json(savedUser);
    } catch (error) {
      console.error(error);
      return res.status(500).json({ error: "Create User Failed" });
    }
  });

//Using Return

  const parseToNumber = (value: unknown) => {
    const number = Number(value);
    if (isNaN(number)) {
      return Error("is not a number");
    }
    return number;
  };
  
  const validateAgeDto = (age: unknown) => {
    const eventuallyAge = parseToNumber(age);
    if (eventuallyAge instanceof Error) {
      return false;
    }
    return true;
  };
  
  console.log(validateAgeDto("10")); // output: true
  console.log(validateAgeDto("hi")); // output: false


  //Result Type

 /* type Success<T> = {
    error: false;
    value: T;
  };
  
  type Failure<U> = {
    error: true;
    value: U;
  };
  
  type Result<T, U> = Success<T> | Failure<U>;
  
  const success = <T>(value: T): Success<T> => ({
    error: false,
    value
  });
  
  const failure = <T>(value: T): Failure<T> => ({
    error: true,
    value
  });*/


const parseToNumber = (value: unknown): Result<number, Error> => {
  const number = Number(value);
  if (isNaN(number)) {
    return failure(Error("is not a number"));
  }
  return success(number);
};

const validateAgeDto = (age: unknown) => {
  const ageResult = parseToNumber(age);
  if (ageResult.error) {
    return false;
  }
  return true;
};

console.log(validateAgeDto("10")); // output: true
console.log(validateAgeDto("hi")); // output: false


app.post("/users", async (req: Request, res: Response) => {
  try {
    // Validate the user dto
    const userDtoResult = userValidator.validate(req.body);
    if(userDtoResult.error) {
      return res.status(400).json({error: "User Dto is Incorrect"});
    }
    const userDto: UserDto = userDtoResult.value;
    
    // Create the user entity
    const userEntityResult = userMapper.fromDtoToEntity(userDto);
    if(userEntityResult.error) {
      logger.critical({message: 'User can not be mapped from dto to entity', payload: userDto})
      return res.status(500).json({error: "Server Error"});
    }
    const userEntity: UserEntity = userEntityResult.value;

    // Save the user entity to database
    const savedUserResult = await userRepository.save(userEntity);
    if(savedUserResult.error) {
      logger.critical({message: 'User can not be saved to db', payload: userEntity})
      return res.status(500).json({error: "Server Error"});
    }
    const savedUser: UserEntity = savedUserResult.value;
    
    return res.status(201).json(savedUser);
  } catch (error) {
    logger.critical({message: 'User can not be added', payload: req.body})
    return res.status(500).json({ error: "Server Error" });
  }
});