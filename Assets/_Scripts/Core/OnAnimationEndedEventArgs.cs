using System;

public class OnAnimationEndedEventArgs : EventArgs
{
    public int AnimationStateHash { get; init; }

    public OnAnimationEndedEventArgs(int animationStateHash) => AnimationStateHash = animationStateHash;
}