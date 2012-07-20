using System;
using System.Data;
using System.Diagnostics;
using Microsoft.WindowsCE.Forms;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;

namespace Pocket1945
{
	/// <summary>
	/// This class is taken from the article series "Gaming with the .NET Compact Framework"
	/// writen by Geoff Schwab. The articles can be found on http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnnetcomp/html/BustThisGame.asp.
	/// The class is used to handle input from the mobile device.
	/// </summary>
	public class Input
	{
		// Modifiers used in RegisterHotKey and UnregisterFunc1 fsModifiers parameters
		protected const uint MOD_ALT = 0x0001;
		protected const uint MOD_CONTROL = 0x0002;
		protected const uint MOD_SHIFT = 0x0004;
		protected const uint MOD_WIN = 0x0008;
		protected const uint MOD_KEYUP = 0x1000;

		// Defines the values for the hardware keys used by RegisterHotKey and UnregisterFunc1
		// as the id parameter
		public ArrayList HardwareKeys { get { return m_hardwareKeys; } }
		protected ArrayList m_hardwareKeys = new ArrayList(7);

		/// <summary>
		/// This function defines a system-wide hot key.
		/// </summary>
		/// <param name="hWnd">[in] Handle to the window that will receive WM_HOTKEY
		/// messages generated by the hot key. The value of this parameter should
		/// not be NULL.</param>
		/// <param name="id">[in] Identifier of the hot key. No other hot key in the
		/// calling thread should have the same identifier. An application must specify
		/// a value in the range 0x0000 through 0xBFFF. A shared dynamic-link library
		/// (DLL) must specify a value in the range 0xC000 through 0xFFFF.</param>
		/// <param name="fsModifiers">[in] Specifies keys that must be pressed in
		/// combination with the key specified by the nVirtKey parameter in order to
		/// generate a WM_HOTKEY message. The fsModifiers parameter can be a combination
		/// of the values defined above.</param>
		/// <param name="vk">[in] Specifies the virtual-key code of the hot key.</param>
		/// <returns>Nonzero indicates success. Zero indicates failure.
		/// To get extended error information, call GetLastError.</returns>
		[DllImport("coredll.dll")]
		protected static extern uint RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

		/// <summary>
		/// This function undefines a system-wide hot key.
		/// </summary>
		/// <param name="fsModifiers">[in] Specifies combination of modifiers that are to
		/// be unregistered. The fsModifiers parameter can be a combination of the values
		/// defined above.</param>
		/// <param name="id">[in] Identifier of the hot key. No other hot key in the
		/// calling thread should have the same identifier. An application must specify
		/// a value in the range 0x0000 through 0xBFFF. A shared dynamic-link library
		/// (DLL) must specify a value in the range 0xC000 through 0xFFFF.</param>
		/// <returns></returns>
		[DllImport("coredll.dll")]
		protected static extern bool UnregisterFunc1(uint fsModifiers, int id); 

		/// <summary>
		/// This function determines whether a key is up or down at the time the
		/// function is called, and whether the key was pressed after a previous call
		/// to GetAsyncKeyState.
		/// </summary>
		/// <param name="vKey">[in] Specifies one of 256 possible virtual-key codes.
		/// For more information, see Virtual-Key Codes.  You can use left- and right-
		/// distinguishing constants to specify certain keys. For more information
		/// about constants, see Remarks.</param>
		/// <returns>If the function succeeds, the return value specifies whether the
		/// key was pressed since the last call to GetAsyncKeyState, and whether the
		/// key is currently up or down. If the most significant bit is set, the key
		/// is down. The least significant bit is not valid in Windows CE, and should
		/// be ignored. GetAsyncKeyState returns the current key state even if a window
		/// in another thread or process currently has the keyboard focus.</returns>
		[DllImport("coredll.dll")]
		protected static extern short GetAsyncKeyState(int vKey); 

		/// <summary>
		/// MessageWindow class that overrides hotkeys.
		/// </summary>
		protected class InputMessageWindow : MessageWindow
		{
			protected const int WM_HOTKEY = 0x0312;

			protected override void WndProc(ref Message msg)
			{
				// Do not process hot keys
				if (msg.Msg != WM_HOTKEY)
				{
					base.WndProc(ref msg);
				}
			}
		}

		/// <summary>
		/// Bitmask used to access if a button is currently pressed.
		/// </summary>
		protected const byte kCurrentMask = 0x01;

		/// <summary>
		/// Bitmask used to access if a button was previously pressed.
		/// </summary>
		protected const byte kPreviousMask = 0x02;

		/// <summary>
		/// Bitmask used to clear button information.
		/// </summary>
		protected const byte kClearMask = 0xfc;

		/// <summary>
		/// Bitmask used to determine if a button is registered.
		/// </summary>
		protected const byte kRegisteredMask = 0x80;

		/// <summary>
		/// Equal to ~kRegisteredMask.
		/// </summary>
		protected const byte kNotRegisteredMask = 0x7f;

		/// <summary>
		/// Equal to ~kPreviousMask.
		/// </summary>
		protected const byte kNotPreviousMask = 0xfd;

		/// <summary>
		/// Amount to left shift a current button bit to place it in
		/// the previous button bit position.
		/// </summary>
		protected const int kCurToPrevLeftShift = 1;

		/// <summary>
		/// Number of keys to track.
		/// </summary>
		protected const int kNumKeys = 256;

		/// <summary>
		/// Array of key states.  These states are tracked using the various bitmasks
		/// defined in this class.
		/// </summary>
		protected byte[] m_keyStates = new byte[kNumKeys];

		/// <summary>
		/// MessageWindow instance used by GXInput to intercept hardware button presses.
		/// </summary>
		protected InputMessageWindow m_msgWindow = null;

		/// <summary>
		/// Creates an instance of GXInput.
		/// </summary>
		public Input()
		{
#if SMARTPHONE
			m_hardwareKeys.Add(193);
			m_hardwareKeys.Add(112);
			m_hardwareKeys.Add(113);
			m_hardwareKeys.Add(198);
			m_hardwareKeys.Add(197);
			m_hardwareKeys.Add(27);
			m_hardwareKeys.Add(13);
#else
			m_hardwareKeys.Add(193);
			m_hardwareKeys.Add(194);
			m_hardwareKeys.Add(195);
			m_hardwareKeys.Add(196);
			m_hardwareKeys.Add(197);
#endif

			// Create an instance of the MessageWindow that overrides hardware buttons
			m_msgWindow = new InputMessageWindow();

			// Unregister functions associated with each hardware key and then
			// register them for this class.
			foreach (int i in m_hardwareKeys)
			{
				UnregisterFunc1(MOD_WIN, i);
				RegisterHotKey(m_msgWindow.Hwnd, i, MOD_WIN, (uint)i);
			}

			// Initialize each key state
			for (int i = 0; i < kNumKeys; i++)
			{
				m_keyStates[i] = 0x00;
			}
		}

		/// <summary>
		/// Update the states of all of the keys
		/// </summary>
		public void Update()
		{
			for (int i = 0; i < kNumKeys; i++)
			{
				// Only update a key if it is registered
				if ((m_keyStates[i] & kRegisteredMask) != 0)
				{
					// Move the current state to the previous state and clear the current
					// state.
					m_keyStates[i] = (byte)((m_keyStates[i] & kClearMask) | ((m_keyStates[i] << kCurToPrevLeftShift) & kPreviousMask));
					if ((GetAsyncKeyState(i) & 0x8000) != 0)
					{
						// If the key is pressed then set the current state
						m_keyStates[i] |= kCurrentMask;
					}
				}
			}
		}

		/// <summary>
		/// Register all of the hardware keys, including the directional pad.
		/// It is required to register a key in order to receive state information on it.
		/// </summary>
		public void RegisterAllHardwareKeys()
		{
			foreach (int i in m_hardwareKeys)
			{
				RegisterKey(i);
			}

			RegisterKey((int)Keys.Up);
			RegisterKey((int)Keys.Down);
			RegisterKey((int)Keys.Left);
			RegisterKey((int)Keys.Right);
		}

		/// <summary>
		/// Register every possible key. It is required to register a key in order
		/// to receive state information on it.
		/// </summary>
		public void RegisterAllKeys()
		{
			for (int i = 0; i < kNumKeys; i++)
			{
				RegisterKey(i);
			}
		}

		/// <summary>
		/// Unregister all of the hardware keys, including the directional pad.
		/// </summary>
		public void UnregisterAllHardwareKeys()
		{
			foreach (int i in m_hardwareKeys)
			{
				UnregisterKey(i);
			}

			UnregisterKey((int)Keys.Up);
			UnregisterKey((int)Keys.Down);
			UnregisterKey((int)Keys.Left);
			UnregisterKey((int)Keys.Right);
		}

		/// <summary>
		/// Unregister every key.
		/// </summary>
		public void UnegisterAllKeys()
		{
			for (int i = 0; i < kNumKeys; i++)
			{
				UnregisterKey(i);
			}
		}

		/// <summary>
		/// Register the key specified for input.
		/// </summary>
		/// <param name="vKey">Virtual key code</param>
		public void RegisterKey(int vKey)
		{
			m_keyStates[vKey] |= kRegisteredMask;
		}

		/// <summary>
		/// Unregister the key specified.
		/// </summary>
		/// <param name="vKey">Virtual key code</param>
		public void UnregisterKey(int vKey)
		{
			m_keyStates[vKey] &= kNotRegisteredMask;
		}

		/// <summary>
		/// Check if the key is currently pressed but was not previously pressed.
		/// </summary>
		/// <param name="vKey">Virtual key code</param>
		/// <returns>true if just pressed, false otherwise</returns>
		public bool KeyJustPressed(int vKey)
		{
			if ((m_keyStates[vKey] & kCurrentMask) != 0 && (m_keyStates[vKey] & kPreviousMask) == 0)
				return true;

			return false;
		}

		/// <summary>
		/// Check if any key is currently pressed but was not previously pressed.
		/// </summary>
		/// <param name="vKey">Virtual key code</param>
		/// <returns>true if just pressed, false otherwise</returns>
		public bool AnyKeyJustPressed()
		{
			foreach (byte mask in m_keyStates)
			{
				if (((mask & kRegisteredMask) != 0) &&
					((mask & kCurrentMask) != 0) &&
					((mask & kPreviousMask) == 0))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Check if the key is currently released but was previously pressed.
		/// </summary>
		/// <param name="vKey">Virtual key code</param>
		/// <returns>true if just released, false otherwise</returns>
		public bool KeyJustReleased(int vKey)
		{
			if ((m_keyStates[vKey] & kCurrentMask) == 0 && (m_keyStates[vKey] & kPreviousMask) != 0)
				return true;

			return false;
		}

		/// <summary>
		/// Check if the key is currently pressed.
		/// </summary>
		/// <param name="vKey">Virtual key code</param>
		/// <returns>true if pressed, false otherwise</returns>
		public bool KeyPressed(int vKey)
		{
			if ((m_keyStates[vKey] & kCurrentMask) != 0)
				return true;

			return false;
		}

		/// <summary>
		/// Check if the key is currently released.
		/// </summary>
		/// <param name="vKey">Virtual key code</param>
		/// <returns>true if released, false otherwise</returns>
		public bool KeyReleased(int vKey)
		{
			if ((m_keyStates[vKey] & kCurrentMask) == 0)
				return true;

			return false;
		}
	}
}
