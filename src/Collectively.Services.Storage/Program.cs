﻿using Collectively.Common.Host;
using Collectively.Messages.Events.Operations;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Framework;
using Collectively.Messages.Events.Users;
using Collectively.Messages.Events.Groups;

namespace Collectively.Services.Storage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(args: args)
                .UseAutofac(Bootstrapper.LifeTimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToEvent<SignedUp>()
                .SubscribeToEvent<SignedIn>()
                .SubscribeToEvent<UsernameChanged>()
                .SubscribeToEvent<AvatarUploaded>()
                .SubscribeToEvent<AvatarRemoved>()
                .SubscribeToEvent<RemarkCreated>()
                .SubscribeToEvent<RemarkDeleted>()
                .SubscribeToEvent<RemarkResolved>()
                .SubscribeToEvent<RemarkProcessed>()
                .SubscribeToEvent<RemarkRenewed>()
                .SubscribeToEvent<RemarkCanceled>()
                .SubscribeToEvent<PhotosToRemarkAdded>()
                .SubscribeToEvent<PhotosFromRemarkRemoved>()
                .SubscribeToEvent<RemarkVoteSubmitted>()
                .SubscribeToEvent<RemarkVoteDeleted>()   
                .SubscribeToEvent<CommentAddedToRemark>()
                .SubscribeToEvent<CommentEditedInRemark>()
                .SubscribeToEvent<CommentDeletedFromRemark>()
                .SubscribeToEvent<RemarkCommentVoteSubmitted>()
                .SubscribeToEvent<RemarkCommentVoteDeleted>()
                .SubscribeToEvent<FavoriteRemarkAdded>()
                .SubscribeToEvent<FavoriteRemarkDeleted>()
                .SubscribeToEvent<RemarkActionTaken>()
                .SubscribeToEvent<RemarkActionCanceled>()             
                .SubscribeToEvent<OperationCreated>()
                .SubscribeToEvent<OperationUpdated>()
                .SubscribeToEvent<RemarkStateDeleted>()
                .SubscribeToEvent<GroupCreated>()
                .SubscribeToEvent<OrganizationCreated>()
                .SubscribeToEvent<AccountDeleted>()
                .SubscribeToEvent<AccountLocked>()
                .SubscribeToEvent<AccountUnlocked>()
                .Build()
                .Run();
        }
    }
}
