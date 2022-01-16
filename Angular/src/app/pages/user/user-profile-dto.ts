export interface RoleDto {
  name: string;
}

export interface UserProfileDto {
  id: string;
  username: string;
  createdOn: string;
  firstName: string | null;
  lastName: string | null;
  email: string | null;
  discord: string | null;
  phoneNumber: string | null;
  role: RoleDto;
  isPublic: boolean;
  showPhoneNumber: boolean;
  showEmail: boolean;
  showDiscord: boolean;
  showFirstName: boolean;
  showLastName: boolean;
  pictureUri: string | null;
  otherLink: string | null;
  isActive: boolean;
  lastLogin: string | null;
}
