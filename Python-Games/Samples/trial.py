import pygame as pg
from pygame.math import Vector2
import random

WHITE = (255, 255, 255)
BLACK = (0, 0, 0)


class Player(pg.sprite.Sprite):

    def __init__(self, pos, left, right, up, down, fire,
                 all_sprites, bullets, enemy_bullets, game, name, walls):
        super().__init__()
        self.image = pg.transform.scale((pg.image.load(name + str(".png"))), (60, 100))
        self.rect = self.image.get_rect(topleft=pos)
        self.vel = Vector2(0, 0)
        self.pos = Vector2(pos)
        self.dt = 0.03
        self.key_left = left
        self.key_right = right
        self.key_up = up
        self.key_down = down
        self.key_fire = fire
        # Store the groups as attributes, so that you can add bullets
        # and use them for the collision detection in the update method.
        self.all_sprites = all_sprites
        self.bullets = bullets
        self.enemy_bullets = enemy_bullets
        self.walls = walls
        self.fire_direction = Vector2(350, 0)
        self.health = 3
        self.game = game
        self.name = name

    def update(self, dt):
        self.dt = dt
        self.pos += self.vel
        self.rect.center = self.pos

        if self.pos.x < 30:
            self.pos.x = 30
        elif self.pos.x > 770:
            self.pos.x = 770
        if self.pos.y < 80:
            self.pos.y = 80
        elif self.pos.y > 550:
            self.pos.y = 550

        # Check if enemy bullets collide with the player, reduce
        # health and kill self if health is <= 0.
        collided_bullets = pg.sprite.spritecollide(self, self.enemy_bullets, True)
        for bullet in collided_bullets:
            self.health -= 1
            self.game.draw_lives()
            if self.health <= 0:
                self.kill()
                self.game.finishTheGame()

        for bullet in self.bullets:
            collided_bullet = pg.sprite.spritecollide(bullet, self.walls, True)
            if collided_bullet:
                bullet.kill()

    def handle_event(self, event):
        if event.type == pg.KEYDOWN:
            if event.key == self.key_left:
                self.vel.x = -90 * self.dt
                self.fire_direction = Vector2(-350, 0)

            elif event.key == self.key_right:
                self.vel.x = 90 * self.dt
                self.fire_direction = Vector2(350, 0)

            elif event.key == self.key_up:
                self.vel.y = -90 * self.dt
                self.fire_direction = Vector2(0, -350)
            elif event.key == self.key_down:
                self.vel.y = 90 * self.dt
                self.fire_direction = Vector2(0, 350)
            elif event.key == self.key_fire:  # Add a bullet to the groups.
                bullet = Bullet(self.rect.center, self.fire_direction)
                self.bullets.add(bullet)
                self.all_sprites.add(bullet)
        elif event.type == pg.KEYUP:
            if event.key == self.key_left and self.vel.x < 0:
                self.vel.x = 0
            elif event.key == self.key_right and self.vel.x > 0:
                self.vel.x = 0
            elif event.key == self.key_up and self.vel.y < 0:
                self.vel.y = 0
            elif event.key == self.key_down and self.vel.y > 0:
                self.vel.y = 0


class Bullet(pg.sprite.Sprite):

    def __init__(self, pos, velocity):
        super().__init__()
        self.image = pg.Surface((5, 5))
        self.image.fill(pg.Color('aquamarine1'))
        self.rect = self.image.get_rect(center=pos)
        self.pos = pos
        self.vel = velocity

    def update(self, dt):
        self.pos += self.vel * dt
        self.rect.center = self.pos


class Wall(pg.sprite.Sprite):
    def __init__(self, pos):
        super().__init__()
        self.image = pg.Surface((15, 60))
        self.image.fill(BLACK)
        self.rect = self.image.get_rect(center=pos)
        self.pos = pos

    def update(self, dt):
        pass

    def kill(self):
        pass


class Game:

    def __init__(self):
        self.fps = 30
        self.done = False
        self.clock = pg.time.Clock()
        self.screen = pg.display.set_mode((800, 600))
        self.bg_color = pg.Color('gray30')
        self.live_image = pg.transform.scale((pg.image.load("Heart.png")), (30, 30))

        # Sprite groups that contain the players and bullets.
        self.all_sprites = pg.sprite.Group()
        self.bullets1 = pg.sprite.Group()  # Will contain bullets of player1.
        self.bullets2 = pg.sprite.Group()  # Will contain bullets of player2.
        self.Walls = pg.sprite.Group()
        player1 = Player(
            (0, 30), pg.K_a, pg.K_d, pg.K_w, pg.K_s, pg.K_SPACE,
            self.all_sprites, self.bullets1, self.bullets2, self, "Fox", self.Walls)  # Pass the groups.
        player2 = Player(
            (800, 600), pg.K_LEFT, pg.K_RIGHT, pg.K_UP, pg.K_DOWN, pg.K_KP0,
            self.all_sprites, self.bullets2, self.bullets1, self, "Squirrel", self.Walls)  # Pass the groups.

        self.all_sprites.add(player1, player2)
        self.players = pg.sprite.Group(player1, player2)

    def run(self):
        pg.mixer.music.load('GameMusic.mp3')
        pg.mixer.music.play(-1)
        self.draw()
        self.draw_lives_texts()
        self.draw_lives()
        self.createWalls()
        while not self.done:
            self.dt = self.clock.tick(self.fps) / 1000
            self.handle_events()
            self.run_logic()
            self.draw()

    def handle_events(self):
        for event in pg.event.get():
            if event.type == pg.QUIT:
                self.done = True
            for player in self.players:
                player.handle_event(event)

    def finishTheGame(self):
        pass

    def run_logic(self):
        self.all_sprites.update(self.dt)

    def draw(self):
        # self.screen.fill(self.bg_color)
        pg.draw.rect(self.screen, BLACK, (0, 0, 800, 30))
        pg.draw.rect(self.screen, self.bg_color, (0, 30, 800, 570))
        self.all_sprites.draw(self.screen)
        pg.display.update(pg.Rect(0, 30, 800, 570))

    def draw_lives_texts(self):
        font = pg.font.SysFont(None, 24)
        img = font.render('Oyun bittiğinde tekrar başlamak icin R\'a tıkla!', True, WHITE)
        self.screen.blit(img, (200, 5))
        for player in self.players:
            if player.name == "Fox":
                img = font.render('Tilki:', True, WHITE)
                self.screen.blit(img, (5, 5))
                pg.display.update((0, 0, 45, 30))
            else:
                img = font.render('Sincap:', True, WHITE)
                self.screen.blit(img, (640, 5))
                pg.display.update(200, 0, 505, 30)

    def draw_lives(self):
        for player in self.players:
            if player.name == "Fox":
                start = 45
            else:
                start = 705
            lives = player.health
            for live in range(lives):
                self.screen.blit(self.live_image, (start + 30 * live, 0))
            pg.display.update(pg.Rect(45, 0, 155, 30))
            pg.display.update(pg.Rect(705, 0, 95, 30))

    def createWalls(self):
        number_of_walls = random.randint(4, 6)
        for i in range(number_of_walls):
            random_x = random.randint(100, 700)
            random_y = random.randint(100, 500)
            new_wall = Wall((random_x, random_y))
            self.Walls.add(new_wall)
            self.all_sprites.add(new_wall)


if __name__ == '__main__':
    pg.init()
    Game().run()
    pg.quit()
