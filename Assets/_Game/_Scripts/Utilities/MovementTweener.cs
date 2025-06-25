using UnityEngine;
using DG.Tweening;
using System;

public static class MovementTweener
{
    public static Sequence ThrowInDirection(Transform target, Vector3 direction, float distance, float height, float duration, Action onComplete = null)
    {
        Vector3 endPos = target.position + direction.normalized * distance;
        return ThrowToPosition(target, endPos, height, duration, onComplete);
    }

    public static Sequence ThrowToPosition(Transform target, Vector3 endPosition, float height, float duration, Action onComplete = null)
    {
        Sequence throwSequence = DOTween.Sequence();

        throwSequence.Append(target.DOJump(
            endValue: endPosition,
            jumpPower: height,
            numJumps: 1,
            duration: duration
        ).SetEase(Ease.OutQuad));

        if (onComplete != null)
        {
            throwSequence.OnComplete(() => onComplete.Invoke());
        }

        return throwSequence;
    }

    public static Sequence ThrowWithSpin(Transform target, Vector3 endPosition, float height, float duration, float spinAmount = 360f, Action onComplete = null)
    {
        Sequence throwSequence = DOTween.Sequence();

        throwSequence.Append(target.DOJump(
            endValue: endPosition,
            jumpPower: height,
            numJumps: 1,
            duration: duration
        ).SetEase(Ease.OutQuad));

        throwSequence.Join(target.DORotate(
            new Vector3(spinAmount, UnityEngine.Random.Range(180f, 360f), 0),
            duration,
            RotateMode.FastBeyond360
        ));

        if (onComplete != null)
        {
            throwSequence.OnComplete(() => onComplete.Invoke());
        }

        return throwSequence;
    }

    public static Sequence PushInDirection(Transform target, Vector3 direction, float distance, float duration, Ease easeType = Ease.OutQuad, Action onComplete = null)
    {
        Vector3 endPos = target.position + direction.normalized * distance;
        return PushToPosition(target, endPos, duration, easeType, onComplete);
    }

    public static Sequence PushToPosition(Transform target, Vector3 endPosition, float duration, Ease easeType = Ease.OutQuad, Action onComplete = null)
    {
        Sequence pushSequence = DOTween.Sequence();

        pushSequence.Append(target.DOMove(endPosition, duration)
            .SetEase(easeType));

        if (onComplete != null)
        {
            pushSequence.OnComplete(() => onComplete.Invoke());
        }

        return pushSequence;
    }

    public static Sequence Knockback(Transform target, Vector3 direction, float force, float duration, float height = 1f, Action onComplete = null)
    {
        Vector3 endPos = target.position + direction.normalized * force;
        Sequence knockbackSequence = DOTween.Sequence();

        knockbackSequence.Append(target.DOJump(
            endValue: endPos,
            jumpPower: height,
            numJumps: 1,
            duration: duration
        ).SetEase(Ease.OutQuad));

        if (onComplete != null)
        {
            knockbackSequence.OnComplete(() => onComplete.Invoke());
        }

        return knockbackSequence;
    }
}