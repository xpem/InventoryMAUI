﻿namespace Services.Interfaces
{
    public interface IBuildDbService
    {
        Task CleanLocalDatabase();
        void Init();
    }
}
