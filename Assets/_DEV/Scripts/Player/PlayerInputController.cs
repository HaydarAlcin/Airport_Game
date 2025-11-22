using UnityEngine;

namespace DEV.Scripts.Player
{
    internal class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private FixedJoystick joystick;

        private Vector2 _moveInput;

        internal Vector2 Move()
        {
            _moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
            return _moveInput;
        }
    }
}