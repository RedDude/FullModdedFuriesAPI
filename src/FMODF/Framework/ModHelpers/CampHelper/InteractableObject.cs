using Brawler2D;
using System;
using CDGEngine;
using cs.Blit;
using Microsoft.Xna.Framework;
using FullModdedFuriesAPI;
using SpriteSystem2;

namespace FullModdedFuriesAPI.Framework.ModHelpers.CampHelper
{
    public class InteractableObject
    {
        private GameController _game;
        private readonly BrawlerGameScreen GameScreen;
        private BrawlerTextObj _actionText;
        private float _elapsedTime;
        private CDGRect _triggerRect;
        private float _textSpeed;
        private float _textHeightLimit;
        private IModHelper _helper;
        private readonly IMonitor _monitor;

        public readonly ContextSensitiveObj ContextObj;
        public BrawlerSpriteObj SpriteObj;
        public bool IsActive = true;
        

        public InteractableObject(GameController game, BrawlerGameScreen gameScreen, IModHelper helper, IMonitor monitor)
        {
            this._game = game;
            this.GameScreen = gameScreen;
            this._helper = helper;
            this._monitor = monitor;

            this._actionText = new BrawlerTextObj("LoveYa15");
            this._actionText.AlignText(TextAlign.Centre, WidthAlign.Centre, HeightAlign.Centre);
            this._actionText.FontSize = 9f;
            this._actionText.dropShadowOffset = new Vector2(2f);

            this._elapsedTime = RNG.get(740).RandomFloat(0.0f, 2f);

            this.ContextObj = new ContextSensitiveObj
            {
                methodObject1 = this, method1Name = "RunAction"
            };



            // this.SpriteObj = new SpriteObj("Camp_Corkboard_Outline_Container");
            this.SpriteObj = new BrawlerSpriteObj("Camp_Corkboard_Outline_Container")
            {
                jumpable = true,
                Opacity = 1,
                addPhysics = true,
                forceAddToStage = true,
                X = -200,
                Y = 105,
                // X = 1005,
                // Y = 305,
                ScaleX = 2,
                ScaleY = 2,
                Rotation = 0,
                Tag = "",
                // isPixelSprite = true,
                // Collidable = true,
                // forceDraw = true,
                // rigidBody = true,
                AnimationSpeed = 0,


                // AnimationSpeed = 0.1f,
                Layer = 12
            };


            // var Assembly = typeof(GameController).Assembly;
            // var Outline_ContainerObjType = Assembly.GetType("Brawler2D.Outline_ContainerObj");
            // (this.SpriteObj as Outline_ContainerObj).disableOutline = false;
            // foreach (object obj in this.SpriteObj.LoadContent())
            //     ;
            // this.SpriteObj.ChangeSprite("Camp_Corkboard_Sprite");

            gameScreen.AddGameObj(this.SpriteObj, 12);
            this.SpriteObj.StopAnimation();

            var trigger = new TriggerObj(this._game, 150, 80);
            this._triggerRect = new CDGRect(
                this.SpriteObj.X - (trigger.Bounds.Width / 2),
                this.SpriteObj.Y - (trigger.Bounds.Height / 2),
                trigger.Bounds.Width, trigger.Bounds.Height);

            // this.SpriteObj.AddStaticHitbox(new Hitbox(this._triggerRect.X,));;
            // this._helper.Reflection.GetField<Hitbox[]>()
            // SpriteObj.HitboxesArray =
            // this.m_globalZones[0].AddTrigger(bossRushPortalTrigger);
            // gameScreen.AddCollHull(new BrawlerCollHullObj((int) this._triggerRect.X, (int) this._triggerRect.Y));
            //onEnter
            // this.SpriteObj.PlayAnimation("PortalLoopStart", "PortalLoopEnd", true, false);
            // this.SpriteObj.hasShadow = true;
            // this.SpriteObj.shadowWidth = 50f;
        }

        public void RunAction()
        {
            this._monitor.Log("Action Did");
        }

        public void UpdateText()
        {
            this._textSpeed = 10f;
            this._textHeightLimit = 2f;

            if (!this.IsActive || this._actionText == null) return;
            this._actionText.Position = this.SpriteObj.Position + new Vector2(0.0f, -115f);
            this._actionText.Y += (float) Math.Sin((double) this._elapsedTime * (double) this._textSpeed) *
                                  this._textHeightLimit;
        }

        public void DrawText(Camera2D camera, float elapsedSeconds)
        {
            if (!this.IsActive || this._actionText == null) return;
            this._actionText.Draw(camera, elapsedSeconds);
        }

        public void UpdateCollisionCheck()
        {
            var hostPlayer = this._game.PlayerManager.hostPlayer;
            var playerManager = this._game.PlayerManager;
            int playerArrayCount = playerManager.activePlayerArray_count;
            for (int index1 = 0; index1 < playerArrayCount; ++index1)
            {
                var activePlayer = playerManager.activePlayerArray[index1];
                // for (int index2 = 0; index2 < this.m_triggerRectList.Count; ++index2)
                if (!this._triggerRect.ContainsMargin(activePlayer.currentPlayerClass.shadowPosition,
                    activePlayer.IsLocal() ? 0.0f : 20f)) continue;

                if (!this.IsActive) continue;

                activePlayer.displayInputIcon = true;
                activePlayer.contextObj = this.ContextObj;

                // activePlayer.currentPlayerClass.Position = activePlayer.currentPlayerClass.Position + activePlayer.currentPlayerClass.collisionEllipse.MTD(
                    // new CDGEllipse(this._triggerRect.X, this._triggerRect.Y, this._triggerRect.Width ,
                        // this._triggerRect.Height));

                // this.m_collisionEllipse.position = this.m_sniper.Position + this.m_sniper.shadowOffset;
                // if (this.m_sniper.Visible && activePlayer.collisionEllipse.Intersects(this.m_collisionEllipse))
                // {
                    // PlayerClassObj playerClassObj = currentPlayerClass;
                    // playerClassObj.Position = playerClassObj.Position + currentPlayerClass.collisionEllipse.MTD(this.m_collisionEllipse);
                // }
//                  if (!this.m_skillTreeObjList[activePlayer.playerIndex].Active && this.m_spriteList[0].Active)
//                    activePlayer.displayInputIcon = true;
            }
        }

    }
}
