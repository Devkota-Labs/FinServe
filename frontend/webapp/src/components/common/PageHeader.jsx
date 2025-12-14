export default function PageHeader({ title, children }) {
  return (
    <div className="flex justify-between items-center mb-6">
      <h2 className="text-2xl font-semibold">{title}</h2>
      <div className="flex gap-3">{children}</div>
    </div>
  );
}
