using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Algebreaker
{
    public class Player
    {
        private const int PlayerWidth = 24; // const meaning that you have to set up value for this attribute IMMEDIATELY
        private const int PlayerHeight = 32;
        private const int _totalFrame = 8;
        private Gun _playerGun;
        private PictureBox _playerPictureBox;
        private Image _spriteSheet;
        private int _currentFrame;
        private int _currentRow;
        private bool _isMoving;
        private MathForm _mathForm;

        public Player(Point startPosition)
        {
            using (MemoryStream ms = new MemoryStream(Resource.rpg_sprite_walk))
            {
                _spriteSheet = Image.FromStream(ms);
            }

            _currentFrame = 0;
            _currentRow = 0;

            _playerPictureBox = new PictureBox
            {
                Size = new Size(48, 64),
                Location = startPosition,
                BackColor = Color.Transparent,
                Image = _spriteSheet,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            _playerGun = new Gun(startPosition);

            UpdateSprite();
        }

        public List<PictureBox> GetPictureBoxes()
        {
            return new List<PictureBox> { _playerPictureBox, _playerGun.GetPictureBox()};
        }

        public PictureBox GetGunPictureBox() => _playerGun.GetPictureBox();

        public void Walk(Keys key, Size boundary, Form form)
        {
            int speed = 10;
            _isMoving = true; // this will be used in Animate(), to signal that yes, I am moving, please animate me

            switch (key)
            {
                case Keys.Down:
                    _currentRow = 0;
                    if (_playerPictureBox.Bottom < boundary.Height)
                    {
                        _playerPictureBox.Top += speed;
                    }
                    break;
                case Keys.Up:
                    _currentRow = 1;
                    if (_playerPictureBox.Top > 0)
                    {
                        _playerPictureBox.Top -= speed;
                    }
                    break;
                case Keys.Left:
                    _currentRow = 2;
                    if (_playerPictureBox.Left > 0)
                    {
                        _playerPictureBox.Left -= speed;
                    }
                    break;
                case Keys.Right:
                    _currentRow = 3;
                    if (_playerPictureBox.Right < boundary.Width)
                    {
                        _playerPictureBox.Left += speed;
                    }
                    break;
                case Keys.Enter:
                    _playerGun.ShootManual(form, _playerPictureBox.Location, _currentRow);  // Pass form to add control for bullets
                    break;
                case Keys.R:
                default:
                    _isMoving = false;
                    break;

            }
            UpdateSprite();
        }

        public void StopWalking()
        {
            _isMoving = false;
            _currentFrame = 0; // so it can be the standing emoji state again
            UpdateSprite();
        }

        public void Animate()
        {
            if (_isMoving)
            {
                //when you move, the index is gonna change, it will keep going left
                _currentFrame = (_currentFrame + 1) % _totalFrame; // dimodul so it can go back to index 0 like a loop 
                UpdateSprite();
            }
        }

        public void UpdateSprite()
        {
            int frameWidth = PlayerWidth;
            int frameHeight = PlayerHeight;

            //Protocol: take the starting x axis, starting y axis and then drag  the frame width and height to make the rectangle 
            Rectangle srcRect = new Rectangle(_currentFrame * PlayerWidth, _currentRow * PlayerHeight, frameWidth, frameHeight);

            //Make it into object of map
            Bitmap currentFrameImage = new Bitmap(frameWidth, frameHeight);

            //Slicer
            using (Graphics g = Graphics.FromImage(currentFrameImage))
            {
                g.DrawImage(_spriteSheet, new Rectangle(0, 0, frameWidth, frameHeight), srcRect, GraphicsUnit.Pixel);
            }

            //Update the Image inside picture Box
            _playerPictureBox.Image = currentFrameImage;
        }
    }
}
