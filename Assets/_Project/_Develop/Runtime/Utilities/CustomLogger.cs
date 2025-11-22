using System;
using UnityEngine;

namespace TestTankProject.Runtime.Utilities
{
    public static class CustomLogger
    {
        public static void Log(string scriptName, string message, 
            MessageTypes messageType, RecipientTypes recipientType = RecipientTypes.DEV) 
        {
            string compiledMessage = $"[{recipientType}]--{scriptName}--: {message}";

            switch (messageType) 
            {
                case MessageTypes.Message:
                    Debug.Log(compiledMessage);
                    break;

                case MessageTypes.Warning:
                    Debug.LogWarning(compiledMessage);
                    break;

                case MessageTypes.Assertion:
                    Debug.LogAssertion(compiledMessage);
                    break;

                case MessageTypes.Error:
                    Debug.LogError(compiledMessage);
                    break;
                
                case MessageTypes.Exception:
                    throw new Exception(compiledMessage);
            }
        }
    }

    public enum MessageTypes 
    {
        Message, 
        Warning,
        Assertion,
        Error, 
        Exception
    }

    public enum RecipientTypes
    {
        DEV,
        GD
    }
}