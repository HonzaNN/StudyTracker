﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudyTracker.DAL;

#nullable disable

namespace StudyTracker.DAL.Migrations
{
    [DbContext(typeof(StudyTrackerDbContext))]
    partial class StudyTrackerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.3");

            modelBuilder.Entity("StudyTracker.DAL.Entities.ActivityEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ActivityCreatorId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("SubjectEntityId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SubjectEntityId");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("StudyTracker.DAL.Entities.ActivityToUserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ActivityId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("UserId");

                    b.ToTable("ActivityToUser");
                });

            modelBuilder.Entity("StudyTracker.DAL.Entities.SubjectEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Shortcut")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("StudyTracker.DAL.Entities.SubjectToUserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.HasIndex("UserId");

                    b.ToTable("SubjectToUser");
                });

            modelBuilder.Entity("StudyTracker.DAL.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CreatorOfActivityId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageUri")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("TeacherToSubjectId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("StudyTracker.DAL.Entities.ActivityEntity", b =>
                {
                    b.HasOne("StudyTracker.DAL.Entities.SubjectEntity", null)
                        .WithMany("Activities")
                        .HasForeignKey("SubjectEntityId");
                });

            modelBuilder.Entity("StudyTracker.DAL.Entities.ActivityToUserEntity", b =>
                {
                    b.HasOne("StudyTracker.DAL.Entities.ActivityEntity", "Activity")
                        .WithMany("Users")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("StudyTracker.DAL.Entities.UserEntity", "User")
                        .WithMany("Activities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Activity");

                    b.Navigation("User");
                });

            modelBuilder.Entity("StudyTracker.DAL.Entities.SubjectToUserEntity", b =>
                {
                    b.HasOne("StudyTracker.DAL.Entities.SubjectEntity", "Subject")
                        .WithMany("Users")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudyTracker.DAL.Entities.UserEntity", "User")
                        .WithMany("Subjects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Subject");

                    b.Navigation("User");
                });

            modelBuilder.Entity("StudyTracker.DAL.Entities.ActivityEntity", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("StudyTracker.DAL.Entities.SubjectEntity", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("StudyTracker.DAL.Entities.UserEntity", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("Subjects");
                });
#pragma warning restore 612, 618
        }
    }
}
