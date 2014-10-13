﻿namespace SantaHo.Domain.Configuration
{
    public interface ISettingsRepository
    {
        TValue Get<TValue>() where TValue: class;
        void Set<TValue>(TValue value) where TValue : class;
    }
}