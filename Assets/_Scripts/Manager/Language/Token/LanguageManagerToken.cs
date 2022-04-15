namespace KitAR.Manager.Language.Token
{
    public static class LanguageManagerToken
    {
        public static class common
        {
            public static string start_token { get { return LanguageManager.Instance.GetTranslation("common", "start_token"); } }
            public static string fps_token { get { return LanguageManager.Instance.GetTranslation("common", "fps_token"); } }
            public static string anchor_reach_token { get { return LanguageManager.Instance.GetTranslation("common", "anchor_reach_token"); } }
            public static string size_token { get { return LanguageManager.Instance.GetTranslation("common", "size_token"); } }
            public static string invalid_anchor_reach_token { get { return LanguageManager.Instance.GetTranslation("common", "invalid_anchor_reach_token"); } }
            public static string invalid_size_token { get { return LanguageManager.Instance.GetTranslation("common", "invalid_size_token"); } }
            public static string move_interaction_token { get { return LanguageManager.Instance.GetTranslation("common", "move_interaction_token"); } }
            public static string look_interaction_token { get { return LanguageManager.Instance.GetTranslation("common", "look_interaction_token"); } }
            public static string exit_interaction_token { get { return LanguageManager.Instance.GetTranslation("common", "exit_interaction_token"); } }
            public static string add_interaction_token { get { return LanguageManager.Instance.GetTranslation("common", "add_interaction_token"); } }
            public static string remove_interaction_token { get { return LanguageManager.Instance.GetTranslation("common", "remove_interaction_token"); } }
        }
    }
}