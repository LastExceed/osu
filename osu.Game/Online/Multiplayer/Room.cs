// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Configuration;
using osu.Game.Users;

namespace osu.Game.Online.Multiplayer
{
    public class Room
    {
        [JsonProperty("id")]
        public Bindable<int?> RoomID { get; private set; } = new Bindable<int?>();

        [JsonProperty("name")]
        public Bindable<string> Name { get; private set; } = new Bindable<string>("My awesome room!");

        [JsonProperty("host")]
        public Bindable<User> Host { get; private set; } = new Bindable<User>();

        [JsonProperty("playlist")]
        public BindableCollection<PlaylistItem> Playlist { get; set; } = new BindableCollection<PlaylistItem>();

        [JsonProperty("channel_id")]
        public Bindable<int> ChannelId { get; private set; } = new Bindable<int>();

        [JsonIgnore]
        public Bindable<TimeSpan> Duration { get; private set; } = new Bindable<TimeSpan>(TimeSpan.FromMinutes(30));

        [JsonIgnore]
        public Bindable<int?> MaxAttempts { get; private set; } = new Bindable<int?>();

        [JsonIgnore]
        public Bindable<RoomStatus> Status { get; private set; } = new Bindable<RoomStatus>(new RoomStatusOpen());

        [JsonIgnore]
        public Bindable<RoomAvailability> Availability { get; private set; } = new Bindable<RoomAvailability>();

        [JsonIgnore]
        public Bindable<GameType> Type { get; private set; } = new Bindable<GameType>(new GameTypeTimeshift());

        [JsonIgnore]
        public Bindable<int?> MaxParticipants { get; private set; } = new Bindable<int?>();

        [JsonIgnore]
        public Bindable<IEnumerable<User>> Participants { get; private set; } = new Bindable<IEnumerable<User>>(Enumerable.Empty<User>());

        [JsonProperty("duration")]
        private int duration
        {
            get => (int)Duration.Value.TotalMinutes;
            set => Duration.Value = TimeSpan.FromMinutes(value);
        }

        // Only supports retrieval for now
        [JsonProperty("ends_at")]
        public Bindable<DateTimeOffset> EndDate { get; private set; } = new Bindable<DateTimeOffset>();

        // Todo: Find a better way to do this (https://github.com/ppy/osu-framework/issues/1930)
        [JsonProperty("max_attempts", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private int? maxAttempts
        {
            get => MaxAttempts;
            set => MaxAttempts.Value = value;
        }

        public void CopyFrom(Room other)
        {
            RoomID.Value = other.RoomID;
            Name.Value = other.Name;
            Host.Value = other.Host;
            Status.Value = other.Status;
            Availability.Value = other.Availability;
            Type.Value = other.Type;
            MaxParticipants.Value = other.MaxParticipants;
            Participants.Value = other.Participants.Value.ToArray();
            EndDate.Value = other.EndDate;

            Playlist.Clear();
            Playlist.AddRange(other.Playlist);
        }

        public bool ShouldSerializeRoomID() => false;
        public bool ShouldSerializeHost() => false;
        public bool ShouldSerializeEndDate() => false;
    }
}
