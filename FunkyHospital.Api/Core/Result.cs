﻿namespace FunkyHospital.Api.Core
{
    public class Result
    {
        public bool Status { get; set; }
        public string ErrorMessage { get; set; }

        public static Result Success()
        {
            return new Result
            {
                Status = true
            };
        }

        public static Result Failure(string message)
        {
            return new Result
            {
                Status = false,
                ErrorMessage = message
            };
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        public static Result<T> Success(T data)
        {
            return new Result<T>
            {
                Status = true,
                Data = data
            };
        }

        public new static Result<T> Failure(string message)
        {
            return new Result<T>
            {
                Status = false,
                ErrorMessage = message
            };
        }
    }
}