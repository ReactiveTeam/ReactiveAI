using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Scheduling
{
    internal class QueuedCommand : IDeferredCommandHandle
    {
        bool _isActive = true;

        CommandStream _stream;
        public float LastExecution;
        public float NextExecution;

        /// <summary>
        ///   The scheduled command this handle refers to.
        /// </summary>
        /// <value>The command.</value>
        public DeferredCommand Command { get; set; }

        /// <summary>
        ///   If true the associated command is still being executed.
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    if (_isActive)
                        RemoveSelfFromQueue();
                    else
                        AddSelfToQueue();
                    _isActive = value;
                }
            }
        }

        public void Pause()
        {
            if (_isActive == false)
                return;

            float time = AITime.Time;
            float num = NextExecution - time;
            LastExecution = num;
            NextExecution = float.PositiveInfinity;
            _stream.Queue.UpdatePriority(this, NextExecution);
        }

        public void Resume()
        {
            if (_isActive == false)
                return;

            float time = AITime.Time;
            LastExecution = time;
            NextExecution = time + Command.ExecutionDelay;
            _stream.Queue.UpdatePriority(this, NextExecution);
        }

        public QueuedCommand(CommandStream stream)
        {
            _stream = stream;
        }

        void AddSelfToQueue()
        {
            float time = AITime.Time;
            LastExecution = time;
            NextExecution = time + Command.ExecutionDelay;
            _stream.Queue.Enqueue(this, NextExecution);
        }

        void RemoveSelfFromQueue()
        {
            if (_stream.Queue.Peek() == this)
                _stream.Queue.Dequeue();
            else
                _stream.Queue.Remove(this);
        }
    }
}
