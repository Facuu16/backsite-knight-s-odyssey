using System.Collections.Generic;
using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    private Dictionary<string, float> _cooldowns = new Dictionary<string, float>();

    public bool IsInCooldown(string cooldownId)
    {
        return _cooldowns.ContainsKey(cooldownId) && Time.time < _cooldowns[cooldownId];
    }

    public void AddCooldown(string cooldownId, float cooldownTime)
    {
        if (_cooldowns.ContainsKey(cooldownId))
        {
            _cooldowns[cooldownId] = Time.time + cooldownTime;
            return;
        }

        _cooldowns.Add(cooldownId, Time.time + cooldownTime);
    }

    public void RemoveCooldown(string cooldownId)
    {
        _cooldowns.Remove(cooldownId);
    }

    public float PendingSeconds(string cooldownId)
    {
        return _cooldowns.ContainsKey(cooldownId) ? Mathf.Max(_cooldowns[cooldownId] - Time.time, 0.0f) : 0.0f;
    }
}
