# Screenshake2D
A 2D screenshake system for Unity including channels, rotation, scaling, and more!

# Camera Setup
This relies on a particular camera setup. The prefab example shows this setup, but it's as follows.

Root Camera Object (All your movement logic should go here.)

->Screenshake (Screenshake only on this node.)

-->Camera (Position should be 0,0,0.)

# How To Use

To shake the screen, simply call Shake() on Screenshake2D. It accepts either a Screenshake2DDescription, which is a serialized object you can put on MonoBehaviours, or a Screenshake2DData, which is a ScriptableObject that you can use to make global shake effects.

For details on what all the variables do, read the tooltips in the inspector or autocomplete.

# Channels

Screenshake2D has support for channels. You can choose not to use a channel, which will treat the shake request as oneshot effects that stack with each call.

# Channel Modes

The current implemented channels modes are as follows.

-RefreshStrength (If requested strength is greater than current strength, the effect will be applied.)

-IgnoreNew (The effect will not be applied if the effect is not finished.)
