export interface UserGamePublicDto {
  id: string
  username: string;
  pictureUri: string;
}

export interface PlatformDto {
  name: string;
  iconUri: string;
}

export interface GamePublicDto {
  userId: string;
  user: UserGamePublicDto;
  isAvailable: boolean;
  name: string;
  platforme: string;
  title: string | null;
  link: string | null;
  description: string | null;
  price: string | null;
  reviews: string | null;
  tumbnailUrl: string | null;
  publicComment: string;
  receivedDate: string;
  givenDate: string;
  givenTo: string;
}
