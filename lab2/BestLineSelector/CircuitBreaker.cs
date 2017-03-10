using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestLineSelector
{
    public enum CircuitState
    {
        Closed,
        Open,
        HalfOpen
    }

    class OpenCircuitException : SystemException {}

    class CircuitBreaker
    {
        int _maxFailureCount;
        TimeSpan _timeout;

        int _currentFailureCount = 0;
        CircuitState _state = CircuitState.Closed;
        long _blockedTill = DateTime.MinValue.Ticks;

        public CircuitBreaker(int maxFailureCount, TimeSpan timeout)
        {
            if (maxFailureCount <= 0)
            {
                throw new ArgumentOutOfRangeException("maxFailureCount", "Value must be greater than zero.");
            }
            if (timeout < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("timeout", "Value must be greater than zero.");
            }

            _maxFailureCount = maxFailureCount;
            _timeout = timeout;
        }

        public TResult Execute<TResult>(Func<TResult> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (_state == CircuitState.Closed)
            {
                return PerformAction(action);
            }
            else if (_state == CircuitState.Open)
            {
                bool timeoutExpired = DateTime.UtcNow.Ticks >= _blockedTill;
                if (timeoutExpired)
                {
                    Console.WriteLine("HALF-Open at " + DateTime.UtcNow.ToString());
                    _state = CircuitState.HalfOpen;
                    return PerformAction(action);
                }
                else
                {
                    throw new OpenCircuitException();
                }
            }
            else // half-open
            {
                return PerformAction(action);
            }
        }

        public TResult PerformAction<TResult>(Func<TResult> action)
        {
            TResult result;
            try
            {
                Console.WriteLine("Perform Action");
                result = action();
            }
            catch (Exception e)
            {
                _currentFailureCount++;
                Console.WriteLine("Failure #" + _currentFailureCount.ToString());
                if (_currentFailureCount >= _maxFailureCount)
                {
                    OpenBreaker();
                }
                throw e;
            }
            CloseBreaker();
            return result;
        }

        public void Execute(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (_state == CircuitState.Closed)
            {
                PerformAction(action);
            }
            else if (_state == CircuitState.Open)
            {
                bool timeoutExpired = DateTime.UtcNow.Ticks >= _blockedTill;
                if (timeoutExpired)
                {
                    _state = CircuitState.HalfOpen;
                    PerformAction(action);
                }
                else
                {
                    throw new OpenCircuitException();
                }
            }
            else // half-open
            {
                PerformAction(action);
            }
        }

        void PerformAction(Action action)
        {
            try
            {
                Console.WriteLine("Perform Action");
                action();
            }
            catch (Exception e)
            {
                _currentFailureCount++;
                Console.WriteLine("Failure #" + _currentFailureCount.ToString());
                if (_currentFailureCount >= _maxFailureCount)
                {
                    OpenBreaker();
                }
                throw e;
            }
            CloseBreaker();
        }

        void OpenBreaker()
        {
            Console.WriteLine("OpenBreaker");
            _state = CircuitState.Open;
            _blockedTill = (DateTime.UtcNow + _timeout).Ticks;
            Console.WriteLine("now: " + DateTime.UtcNow.ToString());
            Console.WriteLine("+ timeout: " + (DateTime.UtcNow + _timeout).ToString());
        }

        void CloseBreaker()
        {
            Console.WriteLine("CloseBreaker");
            _state = CircuitState.Closed;
            _currentFailureCount = 0;
            _blockedTill = DateTime.MinValue.Ticks;
        }
    }
}
