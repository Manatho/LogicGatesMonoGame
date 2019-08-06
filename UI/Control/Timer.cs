using GameEngine.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.UI
{
    public class Repeater
    {
		
        private double _timeSincePressed;
        private float _timeBeforeResetting;
        private float _wait;

        private bool _enabled;

        public Repeater(float repeatTime)
        {
            _timeBeforeResetting = repeatTime;
            _enabled = false;
        }


        public void Update()
        {
            if (_enabled)
				if (InputController.CurrentUiTime - _timeSincePressed > _timeBeforeResetting + _wait)
                {
                    _wait = 0;
					_timeSincePressed = InputController.CurrentUiTime;
                    OnRepeat();
                }
        }

        public void Start()
        {
            _enabled = true;
        }

        public void Start(float wait)
        {
            Start();
            _wait = wait;
			_timeSincePressed = InputController.CurrentUiTime;
        }

        public void Stop()
        {
            _enabled = false;
        }

        public event EventHandler Repeat;
        private void OnRepeat()
        {
            if (Repeat != null)
                Repeat(this, EventArgs.Empty);
        }


		

    }
}
