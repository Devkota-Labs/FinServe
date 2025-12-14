namespace Shared.Common.Helpers;

public static class ConsoleHelper
{
    public static bool WaitConsoleForUserCommand(string stopCommand)
    {
        var sUserCommand = " ";

        while (!sUserCommand.Equals(stopCommand, StringComparison.Ordinal))
        {
            sUserCommand = Console.ReadLine();

            if (string.IsNullOrEmpty(sUserCommand))
            {
                sUserCommand = string.Empty;
            }

            if (sUserCommand.Trim().Equals(stopCommand, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }
}
