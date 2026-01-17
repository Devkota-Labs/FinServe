type Props = {
  userName: string;
  onLogout: () => void;
};

export const AppHeader = ({ userName, onLogout }: Props) => {
  return (
    <header className="h-14 bg-white border-b flex items-center justify-between px-6">
      <span className="font-semibold">FinServe Admin</span>

      <div className="flex items-center gap-4">
        <span className="text-sm">{userName}</span>
        <button onClick={onLogout} className="text-sm text-red-600">
          Logout
        </button>
      </div>
    </header>
  );
};
