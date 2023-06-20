/*using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyTracker.BL.Mappers;
using StudyTracker.BL.Mappers.Interface;
using StudyTracker.DAL.Entities;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Common;
using StudyTracker.Common.Tests;
using StudyTracker.DAL;

namespace StudyTracker.BL.Tests.Mappers
{
    [TestClass()]
    public class ActivityModelMapperTests
    {
        public readonly ISubjectToUserModelMapper _subjectToUserModelMapper;
        public readonly IActivityToUserModelMapper _activityToUserModelMapper;
        [TestMethod()]
        public void MapToListModelTest()
        {
            var dateTime = System.DateTime.Now;
            var idActivity = Guid.NewGuid();
            var idUser = Guid.NewGuid();
            var idSubject = Guid.NewGuid();
            var idTeacher = Guid.NewGuid();
            
            var userTeacherEntity = new UserEntity
            {
                Id = idTeacher,
                Name = "TestTeacherUsr",
                Surname = "TestTeacherSur",
                ImageUri = "TestTeacherUri"
            };

            var userEntity = new UserEntity
            {
                Id = idUser,
                Name = "TestUsr",
                Surname = "TestSur",
                ImageUri = "TestUri"
            };

            var subjectEntity = new SubjectEntity
            {
                Id = idSubject,
                Name = "TestSbj",
                Shortcut = "Sbj",
                TeacherEntity = userTeacherEntity


            };

            var activityEntity = new ActivityEntity
            {
                Id = idActivity,
                Name = "Test",
                StartDate = dateTime,
                EndDate = dateTime,
                State = ActivityStateEntity.Done,
                Type = ActivityTypeEntity.PracticalClass,
                ActivityCreator = userEntity,
                Subject = subjectEntity

            };

            var activityModelMapper = new ActivityModelMapper();
            var activityListModel = activityModelMapper.MapToListModel(activityEntity);
            Assert.IsNotNull(activityListModel);
            Assert.AreEqual(activityListModel.Id, idActivity);
            Assert.AreEqual(activityListModel.Name, "Test");
            Assert.AreEqual(activityListModel.StartDate, dateTime);
            Assert.AreEqual(activityListModel.EndDate, dateTime);
            Assert.AreEqual(activityListModel.State, ActivityStateEntity.Done);
            Assert.AreEqual(activityListModel.Type, ActivityTypeEntity.PracticalClass);
            
        }

        private void checkUserListModel(ICollection<UserListModel> userListModels, ICollection<UserEntity> userEntities)
        {
            Assert.AreEqual(userListModels.Count, userEntities.Count);
            foreach (var userEntity in userEntities)
            {
                var userListModel = userListModels.FirstOrDefault(x => x.Id == userEntity.Id);
                Assert.IsNotNull(userListModel);
                Assert.AreEqual(userListModel.Id, userEntity.Id);
                Assert.AreEqual(userListModel.Name, userEntity.Name);
                Assert.AreEqual(userListModel.Surname, userEntity.Surname);
                Assert.AreEqual(userListModel.ImageUri, userEntity.ImageUri);
            }
        }   

        [TestMethod]
        public void MapToDetailModelTest()
        {
            var dateTime = System.DateTime.Now;

            var idActivity = Guid.NewGuid();
            var idActivityUserGuid = Guid.NewGuid();
            var idUser1 = Guid.NewGuid();
            var idUser2 = Guid.NewGuid();


            var userEntity = new UserEntity
            {
                Id = idActivityUserGuid,
                Name = "TestUsr",
                Surname = "TestSur",
                ImageUri = "TestUri"
            };

            var userEntity1 = new UserEntity
            {
                Id = idUser1,
                Name = "TestUsr1",
                Surname = "TestSur1",
                ImageUri = "TestUri1"
            };

            

            var userEntity2 = new UserEntity
            {
                Id = idUser2,
                Name = "TestUsr2",
                Surname = "TestSur2",
                ImageUri = "TestUri2"
            };


            var idSubject = Guid.NewGuid();
            var idTeacher = Guid.NewGuid();

            var userTeacherEntity = new UserEntity
            {
                Id = idTeacher,
                Name = "TestTeacherUsr",
                Surname = "TestTeacherSur",
                ImageUri = "TestTeacherUri"
            };


            ICollection<ActivityToUserEntity> activityToUserEntities = new List<ActivityToUserEntity>();


            var subjectEntity = new SubjectEntity
            {
                Id = idSubject,
                Name = "TestSbj",
                Shortcut = "Sbj",
                TeacherEntity = userTeacherEntity


            };

            var activityEntity = new ActivityEntity
            {
                Id = idActivity,
                Name = "Test",
                StartDate = dateTime,
                EndDate = dateTime,
                State = ActivityStateEntity.Done,
                Type = ActivityTypeEntity.PracticalClass,
                ActivityCreator = userEntity,
                Users = activityToUserEntities,
                Subject = subjectEntity

            };

            var activityToUserEntity1 = new ActivityToUserEntity
            {
                Id = Guid.NewGuid(),
                ActivityId = idActivity,
                UserId = idUser1,
                User = userEntity1,
                Activity = activityEntity,
            };



            var activityToUserEntity2 = new ActivityToUserEntity
            {
                Id = Guid.NewGuid(),
                ActivityId = idActivity,
                UserId = idUser2,
                User = userEntity2,
                Activity = activityEntity,
                
            };

            
            activityEntity.Users.Add(activityToUserEntity1);
            activityEntity.Users.Add(activityToUserEntity2);

            var activityModelMapper = new ActivityModelMapper(_subjectToUserModelMapper, _activityToUserModelMapper);

            //Potrebuje dat ty entity do pres fakotorky do databaze a z ni je pak vytahnou activityEntitu a z ni se pak vytvori activityDetailModel
            //To nevim jak se dela, mozna bude potreba i upravit to vytvareni entit nahore
            // Davam zde na tvrdo assert.fail
            Assert.Fail();
            
            var activityDetailModel = activityModelMapper.MapToDetailModel(activityEntity);
            Assert.IsNotNull(activityDetailModel);
            Assert.AreEqual(idActivity, activityDetailModel.Id);
            Assert.AreEqual("Test", activityDetailModel.Name);
            Assert.AreEqual(dateTime, activityDetailModel.StartDate);
            Assert.AreEqual(dateTime, activityDetailModel.EndDate);
            Assert.AreEqual(ActivityStateEntity.Done, activityDetailModel.State);
            Assert.AreEqual(ActivityTypeEntity.PracticalClass, activityDetailModel.Type);
            Assert.AreEqual(idActivityUserGuid, activityDetailModel.ActivityCreator.Id);
            Assert.AreEqual("TestUsr", activityDetailModel.ActivityCreator.Name);
            Assert.AreEqual("TestSur", activityDetailModel.ActivityCreator.Surname);
            Assert.AreEqual("TestUri", activityDetailModel.ActivityCreator.ImageUri);

            var userEntities = new List<UserEntity>();
            userEntities.Add(userEntity1);
            userEntities.Add(userEntity2);

            checkUserListModel(activityDetailModel.Users, userEntities);
            */
        }
    }
}