using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroidX.RecyclerView.Widget;
using static Android.Resource;

namespace AuditApp.Adapters
{
    
    public class AreasRecyclerViewAdapter<T, V> : RecyclerView.Adapter where V : RecyclerView.ViewHolder
    {
        // where T is the type of object in the collection
        // V is your ViewHolder
        private readonly Context _context;
        private readonly List<T> _list;
        private readonly Action<T, V, View> _binder;
        private readonly int _viewLayoutId;
        private View itemView;

        public AreasRecyclerViewAdapter(Context context, List<T> list, Action<T, V, View> binder, int viewLayoutId) : base()
        {
            _context = context;
            _list = list;
            _binder = binder;
            _viewLayoutId = viewLayoutId;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = _list[position];
            var h = holder as V;
            _binder(item, h, itemView);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            itemView = LayoutInflater.From(parent.Context).Inflate(_viewLayoutId, parent, false);
            var viewHolder = Activator.CreateInstance(typeof(V), new object[] { itemView }) as V;

            return viewHolder;
        }

        public override int ItemCount => _list.Count;

    }

    

}