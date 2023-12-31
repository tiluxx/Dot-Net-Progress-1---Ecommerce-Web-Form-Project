﻿namespace DotNetTechWebFormProject.Model
{
    public class ResponseStatus
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }

        public ResponseStatus(bool status, string message, string data = "") 
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
